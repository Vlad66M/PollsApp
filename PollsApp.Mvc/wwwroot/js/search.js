
var form = document.getElementById("search-form");
var ckbxActive = document.getElementById("active");
var ckbxNotVoted = document.getElementById("notvoted");
var autocomplete = document.getElementById("autocomplete");
var searchInput = document.getElementById("search");

ckbxActive.addEventListener("change", () => {
    form.submit();

});

ckbxNotVoted.addEventListener("change", () => {
    form.submit();

});


function ajaxCallGetTitles(text, activeState, votedState) {
    $.ajax({

        url:
            '/polls/getpollstitles',

        type: "GET",

        data: { search: text, active: activeState, notvoted: votedState },  

        success: function (data) {
            
            let x = JSON.stringify(data);
            const a = JSON.parse(x);
            autocomplete.innerHTML = "";
            a.forEach(el => {
                let line = createLine(el);
                autocomplete.appendChild(line);
            });
        },

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
