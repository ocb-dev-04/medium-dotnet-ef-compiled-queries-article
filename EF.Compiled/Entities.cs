using System.ComponentModel.DataAnnotations;

namespace EFCore.Compiled;

public class Base
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
}

public class User : Base
{
    [Required]
    public string FullName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string ProfileImage { get; set; }

    #region Relations

    public virtual ICollection<Shop> Shops { get; set; } = new HashSet<Shop>();

    #endregion
}

public class Shop : Base
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string LogoUrl { get; set; }

    #region Relations

    public virtual User Owner { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    public virtual ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
    public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>();

    #endregion
}

public class Product : Base
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Image { get; set; }
    [Required]
    public string Price { get; set; } // "string" just for example, in real app this prop will be an decimal

    #region Relations

    public virtual Shop Shop { get; set; }

    #endregion
}

public class Phone : Base
{
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string AreaCode { get; set; } = "+1";
    public bool HasWhatsApp { get; set; } = true;

    #region Relations

    public virtual Shop Shop { get; set; }

    #endregion
}

public class Location : Base
{
    [Required]
    public string Locale { get; set; }
    public string ShortDescription { get; set; } = string.Empty;

    #region Relations

    public virtual Shop Shop { get; set; }

    #endregion
}
