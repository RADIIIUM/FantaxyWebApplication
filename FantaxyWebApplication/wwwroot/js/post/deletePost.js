$(document).ready(function () {
    $('.deleteBtn').click(function () {
        var $this = $(this);
        var IdPost = $this.data('post-id');

        $.ajax({
            type: 'POST',
            url: '/Post/DeletePost',
            data: { IdPost: IdPost },
            success: function (data) {
                if (data.success) {
                    alert("Пост был удален");
                }
                else {
                    alert("Ошибка");
                }
            }
        });
    });
});



