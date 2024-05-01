
$(document).ready(function () {
    $('.btn').click(function () {
        var $this = $(this);
        var type = $this.data('type');
        var postId = $this.data('post-id');

        $.ajax({
            type: 'POST',
            url: '/Post/LikeOrDislike',
            data: { postId: postId, type: type },
            success: function (data) {
                if (type === 'like') {
                    $this.toggleClass('green');
                    $('.btn.red[data-post-id="' + postId + '"]').removeClass('red');
                    $('.likeCount[data-post-id="' + postId + '"]').text(data.likeCount);
                    $('.dislikeCount[data-post-id="' + postId + '"]').text(data.dislikeCount);

                }
                else {
                    $this.toggleClass('red');
                    $('.btn.green[data-post-id="' + postId + '"]').removeClass('green');
                    $('.likeCount[data-post-id="' + postId + '"]').text(data.likeCount);
                    $('.dislikeCount[data-post-id="' + postId + '"]').text(data.dislikeCount);
                }

            }
        });
    });


    $('.post-likes-dislikes').each(function () {
        var postId = $(this).find('button[data-type="like"]').data('post-id');

        $.ajax({
            type: 'GET',
            url: '/Post/GetLikeDislikeStatus',
            data: { postId: postId },
            success: function (data) {
                if (data.likeStatus) {
                    $(this).find('button[data-type="like"]').addClass('green');
                }
                if (data.dislikeStatus) {
                    $(this).find('button[data-type="dislike"]').addClass('red');
                }
            }
        });
    });
});