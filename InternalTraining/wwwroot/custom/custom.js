$(document).ready(function () {
    $.ajax({
        url: '/Company2/Cart/GetCartCount',
        type: 'GET',
        success: function (data) {
            $('#cart-count').text(data);
        },
        error: function () {
            console.error("Error fetching cart count.");
        }
    });
});
