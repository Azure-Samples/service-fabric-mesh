(function () {
    function votingApplication() {
        const app = this;
        console.log(this);

        app.votes = {};
        app.refresh = function () {
            start = new Date().getTime();
            $("#vote-list").html("");
            $.ajax({
                url: 'api/votes',
                cache: false,
                success: (data, status, xhr) => {
                    app.votes = data;
                    end = new Date().getTime();
                    updateFooter(xhr, end - start);

                    for (let vote in app.votes) {
                        $("#vote-list").append('<div class="col-4 offset-3"><button class="btn btn-outline-dark btn-block voting-btn-add" data-key="'
                            + vote + '">' + vote + ' - ' + app.votes[vote] + '</button> </div>'
                            + ' <div class="col-2"> <button class="btn btn-dark btn-block voting-btn-remove" data-key="' + vote + '">Remove</button></div>');

                        $("button.voting-btn-add", $("#vote-list")).off("click").on("click", (e) => {
                            app.add($(e.target).data("key"));
                        });

                        $("button.voting-btn-remove", $("#vote-list")).off("click").on("click", (e) => {
                            app.remove($(e.target).data("key"));
                        });
                    }
                },
                error: (xhr) => {
                    updateFooter(xhr, 0);
                }
            });
        };

        app.remove = function (item) {
            $.ajax({
                url: 'api/votes/' + item,
                method: "DELETE",
                cache: false,
                success: (response) => {
                    app.refresh();
                },
                error: (xhr) => {
                    updateFooter(xhr, 0);
                }
            });
        };

        app.add = function (item) {
            if (item) {
                $.ajax({
                    url: 'api/votes/' + item,
                    method: "PUT",
                    cache: false,
                    success: (response) => {
                        app.refresh();
                    },
                    error: (xhr) => {
                        updateFooter(xhr, 0);
                    }
                });
            }
        };

        return app;
    }

    const app = new votingApplication();

    $(document).ready(() => {

        app.refresh();

        $("#btnAdd").click(() => {
            app.add($("#txtAdd").val());
        });

    });
})();


/*This function puts HTTP result in the footer */
function updateFooter(http, timeTaken) {
    if (http.status < 299) {
        statusText.innerHTML = 'Reponse:<br />HTTP status ' + http.status + ' ' + http.statusText + ' returned in ' + timeTaken.toString() + ' ms';
    }
    else {
        statusText.innerHTML = 'Error:<br /> An error occured';
    }
}