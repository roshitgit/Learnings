
**** fix to refresh on loading data
if($scope.tableParams!=null){
  $scope.tableParams.$params.count = 0;
}

** all explains the grid refresh issues.
http://stackoverflow.com/questions/31000330/ngtable-reload-unable-to-refresh-ng-table-does-not-show-new-data-second-time
http://stackoverflow.com/questions/23325994/ng-table-not-working-for-dynamic-data

http://stackoverflow.com/questions/25748209/ngtable-my-table-is-not-refresh-after-http-get#
