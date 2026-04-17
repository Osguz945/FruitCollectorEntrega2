# FruitCollector
Proyecto base proporcionado al alumnado de la asignatura de Tecnología de Servidores y Bases de Datos en el grado de Diseño de Videojuegos en la Universidad Europea



## Objeto Fruit Spawner
El componente Interval Spawner se encarga de obtener la información de una fruta al azar desde Selector Behaviour y crear una nueva fruta en la escena gracias al Factory Behaviour cada Interval Seconds. Para que las posiciones de las frutas no se superpongan, se comprueba si en la posición de aparición aleatoria se encuentra algún objeto que pertenezcan a unas de las capas definidas en Blocking Layers.

## Objeto Fruit Selector
El componente Fruit Selector tiene una única función: devolver la información de una de las frutas que se encuentran en la lista de objetos FruitData. Por defecto hay únicamente una fruta disponible, la manzana, cuya información se encuentra en la carpeta de ScriptableObjects. Si se desea crear más FruitData, se debe hacer clic derecho en la ventana de Project y seleccionar Create > Fruit Picker > Fruits > Fruit Data.

## Objeto Fruit Factory
El componente Fruit Factory crea un objeto en la escena según una posición aleatoria. La plantilla del Fruit Prefab se construye en base a la información extraída del FruitData.

## Objeto Save Button
El componente Save Game Service tiene un método que se ejecuta cuando se hace clic sobre la imagen del disquete. Por el momento, el método no tiene lógica de guardado.

## Objeto Player
El componente Player Movement gestiona el movimiento del jugador con la velocidad definida en Move Speed. Para aplicar el movimiento se pueden usar las flechas de dirección del teclado o las teclas WASD.

El componente Player Inventory gestiona cuando el jugador entra en contacto con un objeto coleccionable (IPickable). Por el momento, la lógica de almacenamiento de datos dinámico no está implementada.

Los componentes Player Interactor y Player Interaction Controller se encargan de que el jugador sea capaz de interactuar con los objetos que pertenezcan a la capa definida en Interaction Layer si se encuentra en un radio de Interaction Radius cuando se pulsa la tecla E. 
Cuando se interactúa con un objeto, los componentes asociados en la lista Components To Disable se desactivan, y se vuelven a activar cuando concluye la interacción con el objeto, pulsando de nuevo la tecla E.

## Objeto Chest
El componente Chest gestiona la interacción del jugador con el objeto. El cofre se puede abrir y/o cerrar, dependiendo del momento de la interacción. Por el momento, la lógica de almacenamiento de datos dinámico no está implementada.
