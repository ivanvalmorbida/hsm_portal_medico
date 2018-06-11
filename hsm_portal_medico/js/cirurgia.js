angular.module('CirurgiasApp', ['ngMaterial','angularSoap'])
.factory("CirurgiaService", ['$soap',function($soap){
    var base_url = "http://127.0.0.1:8080/cirurgia.asmx"

    var getCirurgiaCod = function(cod){
            return $soap.post(base_url,"getCirurgiaCod", {strCod: cod})
        }

    return {
        getCirurgiaCod:getCirurgiaCod
    }
}])
.controller('CirurgiasCtrl', function($http, $scope, CirurgiaService) {
    $scope.procedimentos = []

    $scope.addProc = function() {
        $scope.procedimentos.push({
            'procedimento': $scope.procedimento, 'descricao': $scope.descricao
        })
        $scope.procedimento = null
        $scope.descricao = null
    }

    $scope.BuscarCodigo = function() {
        CirurgiaService.getCirurgiaCod($scope.procedimento).then(function(response){
            pro = JSON.parse(response)
            if(pro.length!=0) 
                $scope.descricao = pro[0].Nome
        })
    }
})