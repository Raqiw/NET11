$(function () {
    function updateSortedData(sortingCriteria, isAscending, page, perPage) {
        $.ajax({
            url: '/MovieCollection/UpdateShowAll',
            type: 'GET',
            data: { sortingCriteria: sortingCriteria, isAscending: isAscending, page: page, perPage: perPage },
            success: function (data) {
                $('#showAllContainer').html(data);
            },
            error: function (error) {
                console.error(error);
            }
        });
    }

    $('#sorterCriteria, #sorterDirection').change(function () {
        const selectedSortingCriteria = $('#sorterCriteria').val();
        const selectedIsAscending = $('#sorterDirection').val();

        const page = $('#Page').val();
        const perPage = $('#PerPage').val();

        updateSortedData(selectedSortingCriteria, selectedIsAscending, 1, perPage);
    });

    $(document).on('click', '.page-link', function (e) {
        const selectedSortingCriteria = $('#sorterCriteria').val();
        const selectedIsAscending = $('#sorterDirection').val();

        const page = $(this).data('page');
        const perPage = $('#PerPage').val();

        e.preventDefault();
        updateSortedData(selectedSortingCriteria, selectedIsAscending, page, perPage);

        $('html, body').animate({ scrollTop: 0 }, 'fast');
    });
});
