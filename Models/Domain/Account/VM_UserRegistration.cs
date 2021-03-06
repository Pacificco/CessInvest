﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CessInvest.Models.Domain.Account
{
    public class VM_UserRegistration
    {
        [Display(Name = "Никнейм (должен быть уникальным, виден всем участникам клуба)")]
        [Required(ErrorMessage = "Поле \"Никнейм\" обязательно для заполнения!")]
        public string NicName { get; set; }

        [Display(Name = "Имя (видно только администрации сайта)")]
        [Required(ErrorMessage = "Поле \"Имя\" обязательно для заполнения!")]
        public string Name { get; set; }

        [Display(Name = "Пол")]        
        [Range(1,2, ErrorMessage = "Поле \"Пол\" обязательно для заполнения!")]
        public int Sex { get; set; }

        [Display(Name = "Электронная почта (будет использоваться как логин для входа на сайт)")]
        [Required(ErrorMessage = "Поле \"Email\" обязательно для заполнения!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email!")]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль (минимум 6 символов)")]
        [Required(ErrorMessage = "Задайте пароль!")]
        [MaxLength(16, ErrorMessage = "Максимальная длина пароля 16 символов!")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля 6 символов!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        public string PasswordConfirmed { get; set; }

        [Display(Name = "Подписаться на рассылку клуба")]        
        public bool Subscribed { get; set; }

        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }
    }
}