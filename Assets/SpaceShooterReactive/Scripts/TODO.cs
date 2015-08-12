﻿//write a blog with small pieces of code
//example - class generator



//create scaffolding for MVC (MVP)
//right click -> new MVC (enter name) -> adds <Name>Model, <Name>Controller, <Name>Presenter

//we need a model locator so that the presenters can find the models and bind acordingly
//locate the model by MODEL_NAME+ID (always autoincrement, optional for single instance)
//override for the MODEL_NAME (case where we have two players for example)
//***each model info should have the presenter asociated with the model
//on register model send the presenter id (overideable)
//the id is actualy the presenter id + model id (to handle multiple models situation)

//http://martinfowler.com/eaaDev/uiArchs.html
//http://joel.inpointform.net/software-development/mvvm-vs-mvp-vs-mvc-the-differences-explained/
//http://www.infragistics.com/community/blogs/todd_snyder/archive/2007/10/17/mvc-or-mvp-pattern-whats-the-difference.aspx

//flow: presenter is created -> it locates its model in Start event (or similar)
//if the model is not found new one is created with the default constructor
//it's the presenters responsibility to assign any properties if needed ?
//bind the model property change events to the view
//how to create commands ? example: weapon fire -> presenter fires the weapon and updates the player ammo property - ok or not - more separation ?
//we should probably move any logic to the model
//example: move btn is pressed -> move function is called in the model -> move event is raised and the player ship moves in determined direction
//this is publisher subscriber actually
//so we have two presenters PlayerUI presenter and PlayerShip presenter
//the buton handling logic can be in the PlayerShip presenter, but if we change the input to mouse drag for example we will have to change the PlayerShip presenter
//by separating this the PlayerUI presenter is separate from the scene representation of the player


//do we need to separate the view(s) from the presenter ?
//or should we have multiple presenters for one model ? - YES
//example: player ship has health indicator and we have a health progress bar
//these are two separate views of the same model
