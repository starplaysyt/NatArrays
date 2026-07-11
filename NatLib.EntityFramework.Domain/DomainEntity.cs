using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace NatLib.EntityFramework.Domain;

public abstract class DomainEntity<TKey> where TKey : IComparable<TKey>
{
    [Key, Column("id")]
    public TKey Id { get; set; }
}