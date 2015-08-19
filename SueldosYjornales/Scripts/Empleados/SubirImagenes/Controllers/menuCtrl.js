(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope', '$timeout', '$scope', 'sYjResource', 'Upload'];

    function menuCtrl($rootScope, $timeout, $scope, sYjResource, Upload) {
        var vm = this;
        vm.imagene = {};
        $('#subirImagenes').addClass('active');

        vm.tipoImagenes = sYjResource.tipoImagenes.query();

        $scope.$watch('vm.file', function () {
            vm.upload(vm.file);
            console.log('se cargo en la variable vm.file');
        });
        vm.log = '';

        vm.upload = function (file) {
            if (file) {
                Upload.upload({
                    url: sYjResource.imagenesUrl,
                    fields: {
                        'empleadoID': vm.imagene.empleadoID,
                        'tipoImagenID': vm.imagene.tipoImagene.tipoImagenID
                    },
                    file:file
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    $scope.log = 'progress: ' + progressPercentage + '% ' +
                                evt.config.file.name + '\n' + $scope.log;

                }).success(function (data, status, headers, config) {
                    if (!data.error) {
                        //$scope.archivosMovimiento = {};
                    }
                    vm.data = data;
                    $timeout(function () {
                        //$scope.log = 'file: ' + config.file.name + ', Response: ' + JSON.stringify(data) + '\n' + $scope.log;
                        //$scope.listadoMensajes.push(data);
                    });
                });
            }
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
            vm.imagene.empleadoID = vm.empleadoID;
        });
    }
})();
