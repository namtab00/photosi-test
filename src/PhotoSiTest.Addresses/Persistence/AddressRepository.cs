using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Addresses.Persistence;

public class AddressRepository(AddressesDbContext context) : RepositoryBase<Address, AddressesDbContext>(context), IAddressRepository;
