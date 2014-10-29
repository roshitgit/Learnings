Post request to work on server.
use json2.js for "JSON.stringify" to work

$.ajax({
                    type: "POST",
                    url: "WebService/Service.asmx/AddToPilotGroup",
                    data: JSON.stringify({ ID: ID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        $('#loadingDiv').show();
                    },
                    complete: function () {
                        $('#loadingDiv').hide();
                        EnableButtons();
                    },
                    success: function (data) { alert("data added successfully"); },
                    failure: function (jqXHR, textStatus, errorThrown) {
                        alert(jqXHR.responseText);
                    }
                });
