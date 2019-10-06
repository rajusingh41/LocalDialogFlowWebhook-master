using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalDialogFlowWebhook
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class SubcategoryEntity
    {
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public string CategoryName { get; set; }
    }

    public class KeyValueEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public static class FeedbackData
    {
        public static ICollection<CategoryViewModel> GetCategroyData()
        {
            return new List<CategoryViewModel>
             {
                  new CategoryViewModel{ CategoryId=1,CategoryName="IT"},
                   new CategoryViewModel { CategoryId =2,CategoryName="Admin"},
                   new CategoryViewModel {CategoryId=3, CategoryName="HR"}
             };
        }

        public static ICollection<SubcategoryEntity> GetSubcategories()
        {
            return new List<SubcategoryEntity>
            {
                 new SubcategoryEntity { CategoryId=1, CategoryName="IT", SubcategoryId=1,SubcategoryName="Software"},
                 new SubcategoryEntity {CategoryId=1,CategoryName="IT", SubcategoryId=2,SubcategoryName="WondowFormat"},
                 new SubcategoryEntity {CategoryId=1,CategoryName="IT", SubcategoryId=3,SubcategoryName="Hardware"},
                 new SubcategoryEntity {CategoryId=2,CategoryName="Admin", SubcategoryId=4, SubcategoryName="Admin 1"},
                 new SubcategoryEntity {CategoryId=2,CategoryName="Admin", SubcategoryId=5,SubcategoryName="Admin 2"},
                 new SubcategoryEntity {CategoryId=2,CategoryName="Admin",SubcategoryId=6,SubcategoryName="Admin 3"},
                 new SubcategoryEntity {CategoryId=3,CategoryName="HR", SubcategoryId=7,SubcategoryName="HR 1"},
                 new SubcategoryEntity {CategoryId=3,CategoryName="HR", SubcategoryId=8,SubcategoryName="HR 2" },
                 new SubcategoryEntity {CategoryId=3,CategoryName="HR", SubcategoryId=9,SubcategoryName="HR 3"}
            };
        }

        public static ICollection<KeyValueEntity> GetTicktPriortiy()
        {
            return new List<KeyValueEntity>
             {
                 new KeyValueEntity { Id=1, Text="Heigh"},
                 new KeyValueEntity {Id =2,Text="Low"},
                 new KeyValueEntity {Id=3,Text="Medium"}

             };
        }
    }
}