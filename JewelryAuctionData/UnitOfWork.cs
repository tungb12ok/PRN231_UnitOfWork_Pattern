using JewelryAuctionData.Entity;
using JewelryAuctionData.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace JewelryAuctionData
{
    public class UnitOfWork : IDisposable
    {
        private const string ErrorNotOpenTransaction = "You have not opened a transaction yet!";
        private const string ErrorAlreadyOpenTransaction = "Transaction already open";
        private readonly Net1711_231_7_JewelryAuctionContext _context;
        private bool isTransaction;
        private bool _disposed;

        private AuctionResultRepository _auctionResultRepository;
        private AuctionSectionRepository _auctionSectionRepository;
        private BidderRepository _bidderRepository;
        private CompanyRepository _companyRepository;
        private CustomerRepository _customerRepository;
        private JewelryRepository _jewelryRepository;
        private PaymentRepository _paymentRepository;
        private RequestAuctionRepository _requestAuctionRepository;
        private RequestAuctionDetailsRepository _requestAuctionDetailsRepository;

        public UnitOfWork(Net1711_231_7_JewelryAuctionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        internal Net1711_231_7_JewelryAuctionContext Context => _context;

        public AuctionResultRepository AuctionResultRepository =>
            _auctionResultRepository ??= new AuctionResultRepository(this);

        public AuctionSectionRepository AuctionSectionRepository =>
            _auctionSectionRepository ??= new AuctionSectionRepository(this);

        public BidderRepository BidderRepository =>
            _bidderRepository ??= new BidderRepository(this);

        public CompanyRepository CompanyRepository =>
            _companyRepository ??= new CompanyRepository(this);

        public CustomerRepository CustomerRepository =>
            _customerRepository ??= new CustomerRepository(this);

        public JewelryRepository JewelryRepository =>
            _jewelryRepository ??= new JewelryRepository(this);

        public PaymentRepository PaymentRepository =>
            _paymentRepository ??= new PaymentRepository(this);

        public RequestAuctionRepository RequestAuctionRepository =>
            _requestAuctionRepository ??= new RequestAuctionRepository(this);

        public RequestAuctionDetailsRepository RequestAuctionDetailsRepository =>
            _requestAuctionDetailsRepository ??= new RequestAuctionDetailsRepository(this);

        public bool IsTransaction
        {
            get
            {
                return this.isTransaction;
            }
        }
        public async Task BeginTransactionAsync()
        {
            if (this.isTransaction)
            {
                throw new Exception(ErrorAlreadyOpenTransaction);
            }

            isTransaction = true;
        }

        public async Task CommitTransactionAsync()
        {
            if (!this.isTransaction)
            {
                throw new Exception(ErrorNotOpenTransaction);
            }

            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this.isTransaction = false;
        }

        public async Task RollbackTransactionAsync()
        {
            if (!this.isTransaction)
            {
                throw new Exception(ErrorNotOpenTransaction);
            }

            this.isTransaction = false;

            foreach (var entry in this._context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    this._context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
