﻿using BusinessLayerInterfaces.BusinessModels.Movies;

namespace GamerShop.Models.Movies;

public class ShowMovieCollectionViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; }
    public string AuthorName { get; set; }
    public double Rating { get; set; }
    public string ImagePath { get; set; }
    public ICollection<ShortMovieBlm> Movies { get; set; }
}