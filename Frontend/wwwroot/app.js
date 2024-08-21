angular.module('taskForceApp', [])
    .run(['$http', '$rootScope', '$q', function ($http, $rootScope, $q) {
        // Função para carregar a configuração
        function loadConfig() {
            var deferred = $q.defer();
            $http.get('config.json').then(function (response) {
                $rootScope.apiUrl = response.data.apiUrl;
                deferred.resolve();
            }).catch(function () {
                deferred.reject('Failed to load configuration.');
            });
            return deferred.promise;
        }

        // Carregar configuração e garantir que esteja disponível
        loadConfig().catch(function (error) {
            console.error(error);
        });
    }])
    .controller('TaskForceController', ['$scope', '$http', '$timeout', '$rootScope', function ($scope, $http, $timeout, $rootScope) {
        $scope.taskForce = {};
        $scope.taskForces = [];
        $scope.message = {};

        // Função para criar uma nova Task Force
        $scope.createTaskForce = function (taskForce) {
            if (!$rootScope.apiUrl) {
                console.error('API URL is not defined.');
                return;
            }

            $http.post($rootScope.apiUrl + '/TaskForce/CreateTaskForce', taskForce)
                .then(function (response) {
                    var data = response.data;
                    $scope.message = { type: 'success', text: data.Message };
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                    $scope.taskForce = {};
                    $scope.getTaskForces();
                })
                .catch(function (error) {
                    var errorMessage = (error.data && error.data.Message) || 'Failed.';
                    $scope.message = { type: 'error', text: errorMessage };
                    $scope.details = '';
                    if (error.data && error.data.Data) {
                        angular.forEach(error.data.Data, function (value, key) {
                            $scope.details += key + ': ' + value + '<br>';
                        });
                    }
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                });
        };

        // Função para obter todas as Task Forces
        $scope.getTaskForces = function () {
            if (!$rootScope.apiUrl) {
                console.error('API URL is not defined.');
                return;
            }

            $http.get($rootScope.apiUrl + '/TaskForce/FindById')
                .then(function (response) {
                    $scope.taskForces = response.data.Data;
                })
                .catch(function (error) {
                    console.error('Failed to load task forces:', error);
                });
        };

        // Função para deletar uma Task Force
        $scope.deleteTaskForce = function (id) {
            if (!$rootScope.apiUrl) {
                console.error('API URL is not defined.');
                return;
            }

            $http.delete($rootScope.apiUrl + '/TaskForce/DeleteTaskForce/' + id)
                .then(function (response) {
                    $scope.message = { type: 'success', text: 'Task Force deleted successfully.' };
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                    $scope.getTaskForces(); // Atualizar a lista de Task Forces
                })
                .catch(function (error) {
                    $scope.message = { type: 'error', text: 'Failed to delete task force.' };
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                });
        };

        // Função para atualizar o status de uma Task Force
        $scope.updateTaskForceStatus = function (id, status) {
            if (!$rootScope.apiUrl) {
                console.error('API URL is not defined.');
                return;
            }

            $http.patch($rootScope.apiUrl + '/TaskForce/UpdateTaskForceStatus/' + id + '?status=' + status)
                .then(function (response) {
                    $scope.message = { type: 'success', text: 'Task Force status updated successfully.' };
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                    $scope.getTaskForces(); // Atualizar a lista de Task Forces
                })
                .catch(function (error) {
                    $scope.message = { type: 'error', text: 'Failed to update task force status.' };
                    $timeout(function () {
                        $('#messageModal').modal('show');
                    }, 0);
                });
        };

        // Inicializar a lista de Task Forces após a configuração ser carregada
        $rootScope.$watch('apiUrl', function (newVal) {
            if (newVal) {
                $scope.getTaskForces();
            }
        });
    }]);
