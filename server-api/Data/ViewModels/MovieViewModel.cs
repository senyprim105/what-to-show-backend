using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using server_api.Data.Models;

namespace server_api.Data.ViewModels
{
    public class MovieViewModel
    {
        public Movie Movie{get;set;}
        public IEnumerable<SelectListItem> ImageList {get;set;}
        public IEnumerable<SelectListItem> MovieList {get;set;}
        public IEnumerable<SelectListItem> PersonList{get;set;}
       
    }
}
