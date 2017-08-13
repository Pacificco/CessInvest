using CessInvest.Models.Domain.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CessInvest.Models.Domain.Articles
{
    public class VM_Article
    {
        #region ПОЛЯ И СВОЙСТВА        
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Вы не указали заголовок статьи")]
        [MaxLength(500, ErrorMessage = "Максимальная длина заголовка не должна превышать 500 символов")]
        public string Title { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [MaxLength(255, ErrorMessage = "Максимальная длина подзаголовка не должна превышать 255 символов")]
        [Display(Name = "Подзаголовок")]
        public string SubTitle { get; set; }
        [Display(Name = "Предпросмотр")]
        [Required(ErrorMessage = "Вы не указали краткое описание статьи")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string TextPrev { get; set; }
        [Display(Name = "Полный текст статьи")]
        [Required(ErrorMessage = "Вы не указали полное описание статьи")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string TextFull { get; set; }        
        [Display(Name = "Категория")]
        [Range(1, 10)]
        public int CategoryId { get; set; }
        [Display(Name = "Мета-заголовок")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-заголовка не должна превышать 255 символов")]
        public string MetaTitle { get; set; }        
        [Display(Name = "Мета-описание")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-описания не должна превышать 255 символов")]
        public string MetaDesc { get; set; }
        [Display(Name = "Мета-ключевые слова")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-ключевых слов не должна превышать 255 символов")]
        public string MetaKeys { get; set; }
        [Display(Name = "NoIndex")]
        public bool MetaNoIndex { get; set; }
        [Display(Name = "NoFollow")]
        public bool MetaNoFollow { get; set; }
        [Display(Name = "Дата публикации")]
        [DataType(DataType.Date)]        
        public DateTime PublishedAt { get; set; }        
        [Display(Name = "Дата изменения")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Автор")]
        public int UserId { get; set; }        
        [Display(Name = "Псевдоним")]
        public string OtherUser { get; set; }
        [Display(Name = "Активная")]
        public bool IsActive { get; set; }
        [Display(Name = "Центральная статья")]
        public bool IsCentral { get; set; }
        [Display(Name = "Порядковый номер центральной статьи")]
        [Required(ErrorMessage = "Вы не указали порядковый номер центральной статьи! Если статья не является центральной укажите 0.")]
        [Range(0, 100, ErrorMessage = "Порядковый номер центральной статьи должен быть равен или больше 0 и меньше или равен 100!")]
        public int CentralNumber { get; set; }
        [Display(Name = "Просмотров")]
        public int Hits { get; set; }
                
        [Display(Name = "Css Id")]
        public string CssId { get; set; }
        [Display(Name = "Css классы")]
        public string CssClasses { get; set; }
        [HiddenInput]
        public bool AsSection { get; set; }
        
        public VM_Category Category { get; set; }
        public VM_ArtCommentInfo CommentInfo { get; set; }

        private string _lastError;
        public string LastError { get { return _lastError; } }
        #endregion

        public VM_Article()
        {
            Category = new VM_Category();
            CommentInfo = new VM_ArtCommentInfo();
            _lastError = String.Empty;
            AsSection = false;
        }

        #region МЕТОДЫ

        /// <summary>
        /// Устанавливает значение указанного поля из базы данных
        /// </summary>
        /// <param name="fName">Имя поля</param>
        /// <param name="fValue">Значение поля</param>
        /// <returns>Логическое значение</returns>
        public bool SetFieldValue(string fName, object fValue)
        {
            try
            {
                switch(fName)
                {
                    case "Id":
                        Id = (int)fValue;
                        break;
                    case "Title":
                        Title = (string)fValue;
                        break;
                    case "Alias":
                        Alias = (string)fValue;
                        break;
                    case "SubTitle":
                        SubTitle = (string)fValue;
                        break;
                    case "TextPrev":
                        TextPrev = (string)fValue;
                        break;
                    case "CategoryId":
                        Category.Id = (int)fValue;
                        break;
                    case "IsActive":
                        IsActive = (bool)fValue;
                        break;
                    case "MetaTitle":
                        MetaTitle = (string)fValue;
                        break;
                    case "MetaKeys":
                        MetaKeys = (string)fValue;
                        break;
                    case "MetaDesc":
                        MetaDesc = (string)fValue;
                        break;
                    case "MetaNoIndex":
                        MetaNoIndex = (bool)fValue;
                        break;
                    case "MetaNoFollow":
                        MetaNoFollow = (bool)fValue;
                        break;
                    case "ChangedAt":
                        UpdatedAt = (DateTime)fValue;
                        break;
                    case "CreatedAt":
                        CreatedAt = (DateTime)fValue;
                        break;
                    case "UserId":
                        UserId = (int)fValue;
                        break;
                    case "TextFull":
                        TextFull = (string)fValue;
                        break;
                    case "OtherUser":
                        OtherUser = fValue == DBNull.Value ? String.Empty : (string)fValue;
                        break;
                    case "PublishedAt":
                        PublishedAt = (DateTime)fValue;
                        break;
                    case "Hits":
                        Hits = (int)fValue;
                        break;
                    case "IsCentral":
                        IsCentral = (bool)fValue;
                        break;
                    case "CentralNumber":
                        CentralNumber = fValue == DBNull.Value ? 0 : (int)fValue;
                        break;
                    case "CssId":
                        CssId = fValue == DBNull.Value ? String.Empty : (string)fValue;
                        break;
                    case "CssClasses":
                        CssClasses = fValue == DBNull.Value ? String.Empty : (string)fValue;
                        break;
                    case "catName":
                        Category.Title = (string)fValue;
                        break;
                    case "catAlias":
                        Category.Alias = (string)fValue;
                        break;
                    case "catIsActive":
                        Category.IsActive = (bool)fValue;
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return false;
            }
        }
        
        #endregion
    }
}