﻿(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope', '$timeout', 'sYjResource'];

    function menuCtrl($rootScope, $timeout, sYjResource) {
        var vm = this;
        vm.menu = {};
        vm.menu.introduccion = true;
        $('#historicosEmpleado').addClass('active');
        $('#historicosEmpleado a').attr('href', '#');

        vm.historicoDireccionesFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.historicoDirecciones.class = "active";
            vm.menu.historicoDirecciones.mostrar = true;
        }
        vm.salariosFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.salarios.class = "active";
            vm.menu.salarios.mostrar = true;
        }
        vm.ingresoSalidasFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.ingresoSalidas.class = "active";
            vm.menu.ingresoSalidas.mostrar = true;
        }
        vm.horariosFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.horarios.class = "active";
            vm.menu.horarios.mostrar = true;
        }
        vm.telefonosFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.telefonos.class = "active";
            vm.menu.telefonos.mostrar = true;
        }
        vm.sucursalesFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.sucursales.class = "active";
            vm.menu.sucursales.mostrar = true;
        }

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.historicoDirecciones = {};
            vm.menu.salarios = {};
            vm.menu.ingresoSalidas = {};
            vm.menu.horarios = {};
            vm.menu.telefonos = {};
            vm.menu.sucursales = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
        });
    }
})();
