var asyncFunction = function () {
  var deferred = $.Deferred();

  when($.ajax(), $.ajax()).then(function (data) {
    deferred.resolve(data);
  }, function (error) {
    deferred.reject();
  });

  return deferred.promise();
}

asyncFunction().done(function (data) {
  // async function completed successfully
}).fail(function (error) {
  // async function completed with error(s)
});
