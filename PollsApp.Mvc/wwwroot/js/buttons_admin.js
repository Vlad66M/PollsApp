
document.getElementById("btn-prev").addEventListener("click", () => {
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);
    console.log(urlParams);
    let search = null;
    let active = null;
    let notvoted = null;
    let page = null;
    if (urlParams.has('search')) {
        search = urlParams.get('search');
    }
    if (urlParams.has('active')) {
        active = urlParams.get('active');
    }
    if (urlParams.has('notvoted')) {
        notvoted = urlParams.get('notvoted');
    }
    if (urlParams.has('page')) {
        pageStr = urlParams.get('page');
        page = Number(pageStr);
        page -= 1;
        urlParams.set("page", page);
    }
    else {
        page = 1;
        urlParams.set("page", page);
    }
    window.location.search = urlParams.toString();
    //ajaxCall(search, active, notvoted, page);
});

document.getElementById("btn-next").addEventListener("click", () => {
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);
    console.log(urlParams);
    let search = null;
    let active = null;
    let notvoted = null;
    let page = null;
    if (urlParams.has('search')) {
        search = urlParams.get('search');
    }
    if (urlParams.has('active')) {
        active = urlParams.get('active');
    }
    if (urlParams.has('notvoted')) {
        notvoted = urlParams.get('notvoted');
    }
    if (urlParams.has('page')) {
        pageStr = urlParams.get('page');
        page = Number(pageStr);
        page += 1;
    }
    else {
        page = 2;
    }
    if (urlParams.has('page')) {
        urlParams.set("page", page);
    }
    else {
        urlParams.append("page", page);
    }
    window.location.search = urlParams.toString();


    console.log("btn");

    //ajaxCall(search, active, notvoted, page);

});


function ajaxCall(search, active, notvoted, page) {
    $.ajax({

        url:
            "/admin/getpolls",

        type: "GET",

        data: { search: search, active: active, notvoted: notvoted, page: page },

        success: function (data) {

            document.getElementById("admin-polls-container").innerHTML = data;
        },

        error: function (error) {
            console.log(`Error ${error}`);
        }
    });
}