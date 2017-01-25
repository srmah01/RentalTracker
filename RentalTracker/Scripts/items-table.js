$(function () {
    var trInstance = $('#items-table').find('tr.clickable-row');
    trInstance.click(function () {
        var instance = $(this);
        if (instance.hasClass('row-item-body')) {
            // remove all selected on table
            instance.parent().find('.row-item-body').removeClass('selected');
        }
        else {
            // And on any children
            instance.find('.card-item-body').removeClass('selected');
        }
        var editUrl = instance.find('#urle').attr('href');
        var detailsUrl = instance.find('#urld').attr('href');
        var deleteUrl = instance.find('#urlr').attr('href');

        if (instance.hasClass('row-item-body')) {
            // Operate table rows
            instance.addClass('selected');
        }
        else {
            // operate on cards
            instance.find('.card-item-body').addClass('selected');
        }

        $('#edit').removeClass('disabled').attr('href', editUrl);
        $('#details').removeClass('disabled').attr('href', detailsUrl);
        $('#delete').removeClass('disabled').attr('href', deleteUrl);
    });
});

