# SueldosYjornales
Proyecto de sueldos y Jornales Adaptado a los requerimientos de Paraguay.

Este es una iniciativa particular a la ves que es una forma de aprender a usar esta fascinante herramienta de control de códigos.

A todos los que les interese, este esta hecho con el Framework .NET 4.5.1, ademas con `MVC5, WebApi2 y Angularjs` como principales protagonistas, todo lo hago al momento de comenzar con Visual Studio 2013.

Esto manejará el tema de sueldos y jornales según lo dicte el Ministerio de Justicia y Trabajo del Paraguay.

### Requerimientos
* .NET 4.5.1
* MVC5
* WebApi2
* SqlServer 2014
* AngularJs
* Ui.Bootstrap
* UnderscoreJs

### Capas
La forma en que organizo el código es por capas, y para eso tengo tres capas:
* Presentación
* Application
* Domain

**En la capa de presentación** esta el MVC5 y el WebApi, ademas del HTML, las librerías de Angular y demás librerias Javascript.

**En la capa de Application**, coloque los Dto, que serian los objetos que utilizo para enviar y recibir datos desde el BackEnd hacia el FronEnd y viceversa.

**En la capa de Domain**, coloco todo lo que tiene que ver con la base de datos, es decir el CONTEXT manejado por EntityFramework y los Managers donde coloco la INTELIGENCIA  y los cálculos relacionados con la modificación a los datos.
