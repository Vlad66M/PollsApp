
var form = document.getElementById("search-form");
var ckbxActive = document.getElementById("active");
var ckbxNotVoted = document.getElementById("notvoted");
var autocomplete = document.getElementById("autocomplete");
var searchInput = document.getElementById("search");

ckbxActive.addEventListener("change", () => {
    console.log("ckbx");
    form.submit();

    //ajaxCall();
});

ckbxNotVoted.addEventListener("change", () => {
    console.log("ckbx");
    form.submit();

    //ajaxCall();
});


function ajaxCallGetTitles(text, activeState, votedState) {
    $.ajax({

        // Our sample url to make request 
        url:
            '/polls/getpollstitles',

        // Type of Request
        type: "GET",

        data: { search: text, active: activeState, notvoted: votedState },  

        // Function to call when to
        // request is ok 
        success: function (data) {
            
            let x = JSON.stringify(data);
            const a = JSON.parse(x);
            //console.log(a);
            a.forEach(el => {
                //console.log(el);
                let line = createLine(el);
                autocomplete.appendChild(line);
            });
        },

        // Error handling 
        error: function (error) {
            console.log(`Error ${error}`);
        }
    });
}

function createLine(line) {
    let result = document.createElement("p");
    result.textContent = line;
    result.className = "autocomplete-line";
    addLineEvent(result);
    return result;
}

function fill() {
    
    autocomplete.innerHTML = '';
    let text = searchInput.value;
    let isActive = ckbxActive.checked;
    let notVoted = ckbxNotVoted.checked;

    if (text == "") {
        return;
    }
    console.log("input:");
    console.log(text);
    console.log("active:");
    console.log(isActive);
    ajaxCallGetTitles(text, isActive, notVoted);
}

searchInput.addEventListener("input", () => {

    setTimeout(fill, 100);
    setAutocompleteLocation();
});

function setAutocompleteLocation() {
    
    let rect = searchInput.getBoundingClientRect();

    autocomplete.style.left = rect.left + 'px';
    autocomplete.style.top = (rect.top + 20) + 'px';
}

function addLineEvent(line) {
    line.addEventListener("click", () => {
        searchInput.value = line.textContent;
        form.submit();
    });
}
/*ajaxCall();*/

/*$(document).ready(function () {
    $("#search").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/polls/getpolls",
                type: "GET",
                dataType: "json",
                data: { search: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
});*/