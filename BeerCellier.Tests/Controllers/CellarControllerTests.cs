using BeerCellier.Controllers;
using BeerCellier.Entities;
using BeerCellier.Models;
using BeerCellier.Tests.Fakes;
using Should;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerCellier.Tests.Controllers
{
    class CellarControllerTests
    {
        public void GET_Index_ShouldShowDefaultView()
        {
            // Arrange
            var controller = InitializeController();

            // Act
            var result = controller.Index(null, null) as ViewResult;

            // Assert
            result.ViewName.ShouldBeEmpty();
        }

        public void GET_Index_ShouldShowListOfBeers()
        {
            // Arrange
            var controller = InitializeController();

            // Act
            var result = controller.Index(null, null) as ViewResult;

            // Assert
            ((IEnumerable<BeerViewModel>)result.Model).ShouldNotBeEmpty();
        }        

        public void GET_Index_ShouldOnlyShowBeersForCurrentLoggedUsers()
        {
            // Arrange
            var controller = InitializeController();
            var currentLoggedUser = TestData.Users.First();

            // Act
            var result = controller.Index(null, null) as ViewResult;

            // Assert
            foreach (var beer in TestData.Beers.Where(b => b.Owner.Equals(currentLoggedUser)))
            {
                ((IEnumerable<BeerViewModel>)result.Model).ShouldContain(new BeerViewModel(beer));
            }

            foreach (var beer in TestData.Beers.Where(b => !b.Owner.Equals(currentLoggedUser)))
            {
                ((IEnumerable<BeerViewModel>)result.Model).ShouldNotContain(new BeerViewModel(beer));
            }
        }

        public void GET_Index_BeersShownPerPageShouldBeEqualToPageSize()
        {
            // Arrange
            var controller = InitializeController(2);
            var currentLoggedUser = TestData.Users.First();

            // Act
            var result = controller.Index(null, 1) as ViewResult;

            // Assert
            TestData.Beers.Where(b => b.Owner.Equals(currentLoggedUser)).Count().ShouldBeGreaterThanOrEqualTo(controller.PageSize);
            ((IEnumerable<BeerViewModel>)result.Model).Count().ShouldEqual(controller.PageSize);
        }

        [Input("b")]
        [Input("be")]
        [Input("1")]
        [Input("zzz")]
        public void GET_Index_ShouldShowBeersWithNameThatContainsSearchTerm(string term)
        {
            // Arrange
            var controller = InitializeController();
            var currentLoggedUser = TestData.Users.First();

            // Act
            var result = controller.Index(term, 1) as ViewResult;

            // Assert
            ((IEnumerable<BeerViewModel>)result.Model).Count().ShouldEqual(
                TestData.Beers.Count(b => b.Name.Contains(term) && b.Owner.Equals(currentLoggedUser)));
        }

        public void GET_Details_ShouldShowDetailsForBeer()
        {
            // Arrange
            var controller = InitializeController();
            const int beerID = 2;

            // Act
            var result = controller.Details(beerID) as ViewResult;

            // Assert
            ((BeerViewModel)result.Model).ID.ShouldEqual(beerID);
        }  
        
        public void GET_Create_ShouldShowDefaultView()
        {
            // Arrange
            var controller = InitializeController();

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            result.ViewName.ShouldBeEmpty();
        }      

        public void POST_Create_ShouldAddNewBeer()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new CreateBeerViewModel();

            // Act
            var result = controller.Create(model) as ViewResult;

            // Assert
            persistenceContext.Saved.ShouldBeTrue();
            persistenceContext.Added.Count.ShouldEqual(1);
        }

        public void POST_Create_ShouldNotAddNewBeerIfModelIsNotValid()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new CreateBeerViewModel();

            controller.ModelState.AddModelError("", "");

            // Act
            var result = controller.Create(model) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            persistenceContext.Saved.ShouldBeFalse();
        }

        public void Post_Create_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = InitializeController();
            var model = new CreateBeerViewModel();

            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            result.RouteValues["action"].ShouldEqual("Index");
        }

        public void GET_Edit_ShouldShowDefaultView()
        {
            // Arrange
            var controller = InitializeController();
            var beerId = TestData.Beers.First().ID;

            // Act
            var result = controller.Edit(beerId) as ViewResult;

            // Assert
            result.ViewName.ShouldBeEmpty();
        }

        public void GET_Edit_ShouldShowBeerToBeEdited()
        {
            // Arrange
            var controller = InitializeController();
            var beerID = TestData.Beers.First().ID;

            // Act
            var result = controller.Edit(beerID) as ViewResult;

            // Assert
            ((EditBeerViewModel)result.Model).ID.ShouldEqual(beerID);
        }

        public void POST_Edit_ShouldSaveBeerChanges()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new EditBeerViewModel { ID = 1, Name = "beer 100", Quantity = 12 };

            // Act
            var result = controller.Edit(model) as ViewResult;

            // Assert
            persistenceContext.Saved.ShouldBeTrue();
        }

        public void POST_Edit_ShouldNotSaveBeerChangesIfModelIsNotValid()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new EditBeerViewModel();

            controller.ModelState.AddModelError("", "");

            // Act
            var result = controller.Edit(model) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            persistenceContext.Saved.ShouldBeFalse();
        }

        public void POST_Edit_ShouldReturnNotFoundIfBeerDoesNotExists()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new EditBeerViewModel();

            // Act
            var result = controller.Edit(model);

            // Assert
            result.ShouldBeType<HttpNotFoundResult>();
            persistenceContext.Saved.ShouldBeFalse();
        }

        public void Post_Edit_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = InitializeController();
            var model = new EditBeerViewModel(TestData.Beers.First());

            // Act
            var result = controller.Edit(model) as RedirectToRouteResult;

            // Assert
            result.RouteValues["action"].ShouldEqual("Index");
        }

        public void GET_Delete_ShouldShowDefaultView()
        {
            // Arrange
            var controller = InitializeController();
            var beerID = TestData.Beers.First().ID;

            // Act            
            var result = controller.Delete(beerID) as ViewResult;

            // Assert
            result.ViewName.ShouldBeEmpty();
        }

        public void GET_Delete_ShouldShowBeerToBeDeleted()
        {
            // Arrange
            var controller = InitializeController();
            var beerID = TestData.Beers.First().ID;

            // Act            
            var result = controller.Delete(beerID) as ViewResult;

            // Assert
            ((BeerViewModel)result.Model).ID.ShouldEqual(beerID);
        }

        public void Post_Delete_ShouldDeleteBeer()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new DeleteBeerViewModel { ID = TestData.Beers.First().ID };

            // Act            
            var result = controller.Delete(model) as ViewResult;

            // Assert
            persistenceContext.Saved.ShouldBeTrue();
            persistenceContext.Removed.Count.ShouldEqual(1);

        }

        public void Post_Delete_ShouldReturnNotFoundIfBeerDoesNotExists()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new DeleteBeerViewModel { ID = 0 };

            // Act            
            var result = controller.Delete(model);

            // Assert
            result.ShouldBeType<HttpNotFoundResult>();
            persistenceContext.Saved.ShouldBeFalse();

        }

        public void Post_Delete_ShouldRedirectToIndex()
        {
            // Arrange            
            var controller = InitializeController();
            var model = new DeleteBeerViewModel { ID = TestData.Beers.First().ID };

            // Act            
            var result = controller.Delete(model) as RedirectToRouteResult;

            // Assert
            result.RouteValues["action"].ShouldEqual("Index");
        }        

        public void GET_Drink_ShouldShowDefaultView()
        {
            // Arrange
            var controller = InitializeController();
            var beerId = TestData.Beers.First().ID;

            // Act
            var result = controller.Drink(beerId) as ViewResult;

            // Assert
            result.ViewName.ShouldBeEmpty();
        }

        public void GET_Drink_ShouldShowBeerToBeDrunk()
        {
            // Arrange
            var controller = InitializeController();
            var beerID = TestData.Beers.First().ID;

            // Act
            var result = controller.Drink(beerID) as ViewResult;

            // Assert
            ((BeerViewModel)result.Model).ID.ShouldEqual(beerID);
        }

        public void POST_Drink_ShouldDecreaseQuantityByOneAndSaveChanges()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var beer = TestData.Beers.First();
            var model = new DrinkBeerViewModel { ID = beer.ID };
            var qtyBefore = beer.Quantity;

            // Act
            var result = controller.Drink(model) as ViewResult;

            // Assert
            persistenceContext.Saved.ShouldBeTrue();
            beer.Quantity.ShouldEqual(qtyBefore - 1);
        }

        public void POST_Drink_ShouldNotDecreaseQuantityIfStocksAreAlreadyEmpty()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var beer = TestData.Beers.First();
            var model = new DrinkBeerViewModel { ID = beer.ID };

            beer.Quantity = 0;

            // Act
            var result = controller.Drink(model) as ViewResult;

            // Assert
            persistenceContext.Saved.ShouldBeTrue();
            beer.Quantity.ShouldEqual(0);
        }

        public void POST_Drink_ShouldReturnNotFoundIfBeerDoesNotExists()
        {
            // Arrange
            var persistenceContext = new FakePersistenceContext();
            var controller = InitializeController(persistenceContext);
            var model = new DrinkBeerViewModel { ID = 0 };

            // Act
            var result = controller.Drink(model);

            // Assert
            result.ShouldBeType<HttpNotFoundResult>();
            persistenceContext.Saved.ShouldBeFalse();
        }

        public void Post_Drink_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = InitializeController();
            var model = new DrinkBeerViewModel { ID = TestData.Beers.First().ID };

            // Act
            var result = controller.Drink(model) as RedirectToRouteResult;

            // Assert
            result.RouteValues["action"].ShouldEqual("Index");
        }

        [Input("b")]
        [Input("be")]
        [Input("1")]
        [Input("zzz")]
        public void GET_QuickSearch_ShouldReturnBeersWithNameThatStartsWithSearchTerm(string term)
        {
            // Arrange
            var controller = InitializeController();
            var currentLoggedUser = TestData.Users.First();            

            // Act
            var result = controller.QuickSearch(term) as JsonResult;
            
            // Assert
            ((IEnumerable<object>)result.Data).Count().ShouldEqual(
                TestData.Beers.Count(b => b.Name.StartsWith(term) && b.Owner.Equals(currentLoggedUser)));
        }

        private static CellarController InitializeController(FakePersistenceContext persistenceContext, int pageSize = 5)
        {
            persistenceContext.AddSet(TestData.Users);
            persistenceContext.AddSet(TestData.Beers);

            var sessionContext = new FakeSessionContext(TestData.Users.First());

            CellarController controller = new CellarController(persistenceContext, sessionContext, pageSize);
            controller.ControllerContext = new FakeControllerContext();

            return controller;
        }        

        private static CellarController InitializeController(int pageSize = 5)
        {
            return InitializeController(new FakePersistenceContext(), pageSize);
        }       
    }

    static class TestData
    {
        public static IQueryable<User> Users { get; private set; }
        public static IQueryable<Beer> Beers { get; private set; }

        static TestData()
        {
            var users = new List<User>();
            var user1 = new User("user1", "my super secret password") { ID = 1 };
            var user2 = new User("user2", "my super secret password") { ID = 2 };

            users.Add(user1);
            users.Add(user2);

            Users = users.AsQueryable();

            var beers = new List<Beer>();

            beers.AddRange(new Beer[]
            {
                new Beer
                {
                    ID = 1,
                    Name = "beer 1",
                    Quantity = 10,
                    Owner = user1
                },
                new Beer
                {
                    ID = 2,
                    Name = "beer 2",
                    Quantity = 10,
                    Owner = user1
                },
                new Beer
                {
                    ID = 3,
                    Name = "beer 3",
                    Quantity = 10,
                    Owner = user1
                },
                new Beer
                {
                    ID = 4,
                    Name = "beer 4",
                    Quantity = 10,
                    Owner = user2
                },
                new Beer
                {
                    ID = 5,
                    Name = "beer 5",
                    Quantity = 10,
                    Owner = user2
                },
                new Beer
                {
                    ID = 6,
                    Name = "beer 10",
                    Quantity = 10,
                    Owner = user1
                }
            });

            Beers = beers.AsQueryable();
        }
    }
}
