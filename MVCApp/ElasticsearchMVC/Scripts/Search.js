$(function () {
    let BooleanDiv = $("#BooleanDiv");
    let PhrazeDiv = $("#PhrazeDiv");
    let FuzzyDiv = $("#FuzzyDiv");

    BooleanDiv.hide();
    FuzzyDiv.hide();

    $("#querySelect").change(function () {
        let query = this.value;

        if (query == "Boolean") {
            BooleanDiv.show();
            FuzzyDiv.hide();
            PhrazeDiv.hide();
        }
        else if (query == "Phraze query") {
            PhrazeDiv.show();
            BooleanDiv.hide();
            FuzzyDiv.hide();
        }
        else if (query == "Fuzzy query") {
            FuzzyDiv.show();
            BooleanDiv.hide();
            PhrazeDiv.hide();
        }
    });
});

function Search() {
    let searchInput = $("#searchInput").val();
    let querySelect = $('#querySelect').find(":selected").text();

    let andOrNot = $('input[name=andOrNot]:checked').val()

    let slop = $("#slop").val();

    let fuzziness = $("#fuzziness").val();
    let prefixLenght = $("#prefixLenght").val();
    let transpositionsboxes = $('input[name=transpositions]:checked').val();
    let maxExpansions = $("#maxExpansions").val();

    let transposition = false;

    if (transpositionsboxes == "on")
        transposition = true;

    if ($("#querySelect").val() == "Boolean") {
        $.ajax({
            url: 'https://localhost:62759/Elasticsearch/bool/' + searchInput + "/" + andOrNot,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                fillTable(response);
                hideColumns();
            },
            error: function (response, jqXHR) {
            }
        });
    }
    else if ($("#querySelect").val() == "Phraze query") {

        if (slop == "")
            slop = 1;

        $.ajax({
            url: 'https://localhost:62759/Elasticsearch/phrase/' + searchInput + "/" + slop,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                fillTable(response);
                hideColumns();
            },
            error: function (response, jqXHR) {
            }
        });
    }
    else if ($("#querySelect").val() == "Fuzzy query") {

        if (fuzziness == "")
            fuzziness = 2;
        if (prefixLenght == "")
            prefixLenght = 0;
        if (maxExpansions == "")
            maxExpansions = 100;

        console.log(searchInput);

        $.ajax({
            url: 'https://localhost:62759/Elasticsearch/fuzzy/' + searchInput + "/" + fuzziness + "/" + prefixLenght
                + "/" + transposition + "/" + maxExpansions,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                fillTable(response);
                hideColumns();
            },
            error: function (response, jqXHR) {
            }
        });
    }
}

function fillTable(data) {
    let tableBody = $("#tableBody");
    let hightlights = "";

    tableBody.empty();
    for (var i = 0; i < data.length; i++) {
        for (var j = 0; j < data[i].hightlights.length; j++) {
            hightlights += data[i].hightlights[j] + "<br />";
        }

        tableBody.append(`
		<tr>
			<td><a class="ahref" href="/Files/Details/${data[i].FileId}">${data[i].FileTitle}</a></td>
			<td>${hightlights}</td>
			
		</tr>
		`);
    }
}


