
$("#ticket, #day , #CategoryId").change(filtering).ready(filtering);

function filtering () {
    $.ajax({
        url: '/MyEvents/FilterCreatedEvents',
        type: "GET",
        cache: false,
        dataType: 'html',
        data: { Price: $('#ticket').val(), Day: $('#day').val(), CategoryId: $('#CategoryId').val() },
        success: function (returnedEvents) {
            $("#partial").html(returnedEvents);
        },
        error: function (xhr, textStatus, errorThrown) {
            alert("failed");
            console.log('STATUS: ' + textStatus + '\nERROR THROWN: ' + errorThrown);
        }
    })
}
