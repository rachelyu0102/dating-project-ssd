var url = 'http://instadateapi.zaichaopan.com/api/Region';

function find() {
    var id = $('#provinceFind').val();
    $.getJSON(url + "/" + id,
        function (data) {
            if (data.clients.length == 0) {
                $('#clientsResult').text('clients not found.');
                return;
            }

            callbackClients(data);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#clientsResult').text('Find error: ' + err);
        });
}




function callbackClients(data) {
    var valTag = document.getElementById("clientsResult");
    valTag.innerHTML = "";
    var table = document.createElement("table");
    valTag.appendChild(table);
    table.className = "table table-striped table-hover table-responsive";
    var caption = document.createElement("caption");
    caption.innerHTML = data.province +": " +data.totalPeople +" people" + "   " +"male: " + data.male + " " + "female: " +data.female; 
    table.appendChild(caption);
    var tr1 = document.createElement("tr");
    table.appendChild(tr1);
    var th1 = document.createElement("th");
    th1.innerHTML = "Username";
    tr1.appendChild(th1);
    var th2 = document.createElement("th");
    th2.innerHTML = "Gender";
    tr1.appendChild(th2);
    var th3 = document.createElement("th");
    th3.innerHTML = "Age";
    tr1.appendChild(th3);

    data.clients.forEach(function (val) {
        var tr2 = document.createElement("tr");
        table.appendChild(tr2);
        var td1 = document.createElement("td");
        td1.innerHTML = val.Username;
        tr2.appendChild(td1);
        var td2 = document.createElement("td");
        td2.innerHTML = val.Gender;
        tr2.appendChild(td2);
        var td3 = document.createElement("td");
        var now = new Date();
        td3.innerHTML = parseInt((now - Date.parse(val.Age)) / (1000 * 60 * 60 * 24 * 365));
        tr2.appendChild(td3);
    });
}

