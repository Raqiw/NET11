﻿using BusinessLayerInterfaces.BusinessModels.Movies;
using BusinessLayerInterfaces.MovieServices;
using GamerShop.Models.Movies;
using GamerShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using DALInterfaces.Models.Movies;
using IronPdf.Pages;

namespace GamerShop.Controllers.Movies;

public class MovieCollectionController : Controller
{
    private readonly IMovieCollectionService _collectionService;
    private readonly IMovieServices _movieServices;
    private readonly IAuthService _authService;
    private readonly IPaginatorService _paginatorService;

    public MovieCollectionController(IMovieCollectionService collectionService, IMovieServices movieServices,
        IAuthService authService, IPaginatorService paginatorService)
    {
        _collectionService = collectionService;
        _movieServices = movieServices;
        _authService = authService;
        _paginatorService = paginatorService;
    }

    [HttpGet]
    public IActionResult Show(int id)
    {
        var collectionBlm = _collectionService.GetMovieCollectionById(id);
        var collectionViewModel = new ShowMovieCollectionViewModel
        {
            Id = collectionBlm.Id,
            Title = collectionBlm.Title,
            Description = collectionBlm.Description,
            DateCreated = collectionBlm.DateCreated,
            AuthorName = collectionBlm.AuthorName,
            Movies = collectionBlm.Movies,
            Rating = collectionBlm.Rating,
            ImagePath = collectionBlm.ImagePath
        };

        return View(collectionViewModel);
    }

    [HttpGet]
    public IActionResult ShowAll(int page = 1, int perPage = 5, string sortingCriteria = "Newest", bool isAscending = true)
    {
        var filter = (Expression<Func<Collection, bool>>)(x => x.IsPublic == true);
        var paginatorViewModel = _paginatorService.GetPaginatorViewModelWithFilter(
            _collectionService,
            MapBlmToViewModel,
            filter,
            sortingCriteria,
            page,
            perPage,
            isAscending);

        return View(paginatorViewModel);
    }

    [HttpGet]
    public IActionResult UpdateShowAll(int page = 1, int perPage = 3, string sortingCriteria = "Newest", bool isAscending = true)
    {
        var filter = (Expression<Func<Collection, bool>>)(x => x.IsPublic == true);
        var paginatorViewModel = _paginatorService.GetPaginatorViewModelWithFilter(
            _collectionService,
            MapBlmToViewModel,
            filter,
            sortingCriteria,
            page,
            perPage,
            isAscending);

        return PartialView("_UpdateShowAllPartialViewModel", paginatorViewModel);
    }


    private ShowShortMovieCollectionViewModel MapBlmToViewModel(ShortMovieCollectionBlm shortMovieCollectionBlm)
    {
        return new ShowShortMovieCollectionViewModel
        {
            Id = shortMovieCollectionBlm.Id,
            Title = shortMovieCollectionBlm.Title,
            Description = shortMovieCollectionBlm.Description,
            DateCreated = shortMovieCollectionBlm.DateCreated,
            Rating = shortMovieCollectionBlm.Rating
        };
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        var createCollectionViewModel = new CreateMovieCollectionViewModel
        {
            AvailableMovies = _movieServices.GetAvailableMoviesForSelection()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Title,
                    Selected = false
                })
                .ToList()
        };
        return View(createCollectionViewModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(CreateMovieCollectionViewModel createMovieCollectionViewModel, IFormFile imageFile)
    {
        if (!ModelState.IsValid) return View(createMovieCollectionViewModel);
        string rdyImagePath = null;

        if (imageFile != null && imageFile.Length > 0)
        {
            rdyImagePath = SaveImage(imageFile);
        }
        else
        {
            rdyImagePath = "/img/MovieCollection/film-reel.png";
        }

        var movieCollectionBlmForCreate = new MovieCollectionBlmForCreate
        {
            Title = createMovieCollectionViewModel.Title,
            Description = createMovieCollectionViewModel.Description,
            IsPublic = createMovieCollectionViewModel.IsPublic,
            MoviesIds = createMovieCollectionViewModel
                .AvailableMovies
                .Where(s => s.Selected)
                .Select(s => int.Parse(s.Value))
                .ToList(),
            Author = _authService.GetCurrentUser(),
            ImagePath = rdyImagePath,
        };

        _collectionService.CreateMovieCollection(movieCollectionBlmForCreate);
        return RedirectToAction("Show", "MovieMain");
    }
    private string SaveImage(IFormFile image)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        var imagePath = Path.Combine("wwwroot", "img", "MovieCollection", fileName);

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        return $"/img/MovieCollection/{fileName}";
    }
}