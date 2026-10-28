
$(function () {
    // Confirm before submitting a maintenance request status update
    $(".status-form").on("submit", function (event) {
        const confirmed = confirm(
            "Are you sure you want to update this maintenance request?"
        );

        if (!confirmed) {
            event.preventDefault();
        }
    });

    // Filter maintenance requests by the text entered
    $("#requestSearch").on("input", function () {
        const searchText = $(this).val().toLowerCase();

        $(".request-card").each(function () {
            const requestText = $(this).text().toLowerCase();

            $(this).toggle(requestText.includes(searchText));
        });
    });
});
