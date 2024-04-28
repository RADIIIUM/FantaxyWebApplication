
document.addEventListener("DOMContentLoaded", function () {
    var createPostButton = document.getElementById("createPostButton");
    var createPostModal = document.getElementById("createPost");
    var postImgForm = document.getElementById('postImgForm');
    var modalImages = document.getElementById('modal-images');
    var postImgInput = document.getElementById('postImgInput');


    // ���������� ������� ��� �������� �����������
    postImgInput.addEventListener('change', function (event) {
        event.preventDefault();

        var postImgForm = document.getElementById('postImgForm');
        var formData = new FormData(postImgForm);
            var images = [];
        fetch('/Post/PostUploadFile', { // ����� ������ ��������������� ������ ��������
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.images && data.images.length <= 3) {
                    // ��������� ����������� �� ��������
                    updateImages(data.images);
                } else if (data.images) {
                    alert("������ ��������� ����� 3 �����������");
                }
            })
            .catch(error => console.error('Error:', error));
    });



    // ������� ��� ���������� ����������� �� ��������
    function updateImages(images) {
        if (checkImageCount()) {
            modalImages.innerHTML = "";
            images.forEach(image => { // ������������ ���������� ����������� �����
                var imgElement = document.createElement('img');
                imgElement.classList.add('postImage');
                imgElement.setAttribute('data-tooltip', '������� �����������');
                imgElement.src = `data:image/jpeg;base64,${image.toString('base64')}`;
                imgElement.addEventListener('click', function () {
                    this.parentNode.removeChild(this);
                });

                modalImages.appendChild(imgElement);
            });
        }
        else {
            alert('������ ��������� ����� 3 �����������');
        }

    }



    function checkImageCount() {
        var images = modalImages.getElementsByTagName('img');
        if (images.length < 3) return true;
        else return false;
    }

    // ������� ��� �������� �����������
    function deleteImage(imageIndex) {
        fetch('/Post/DeleteImage?index=' + imageIndex, { // ����� ������ ��������������� ������ ��������
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                if (data.result === 'success') {
                    // ��������� ����������� �� ��������
                    updateImages(data.images);
                }
            })
            .catch(error => console.error('Error:', error));
    }
    createPostButton.addEventListener("click", function () {
        createPostModal.style.display = "block";
    });

    window.addEventListener("click", function (event) {
        if (event.target == createPostModal) {
            createPostModal.style.display = "none";
        }
    });
});


   

    //