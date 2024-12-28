using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFunkosCS.FunkoNamespace.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiFunkosCS.CategoryNamespace.Model;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
}