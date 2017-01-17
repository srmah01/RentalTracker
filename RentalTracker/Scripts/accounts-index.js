$(function () {
    var trInstance = $('#accounts').find('tr.clickable-row');
    trInstance.click(function () {
        trInstance.find('.card-item-body').removeClass('selected');
        var instance = $(this);
        var editUrl = instance.find('#urle').attr('href');
        var detailsUrl = instance.find('#urld').attr('href');

        instance.find('.card-item-body').addClass('selected');

        $('#edit').removeClass('disabled').attr('href', editUrl);
        $('#details').removeClass('disabled').attr('href', editUrl);
    });
});

