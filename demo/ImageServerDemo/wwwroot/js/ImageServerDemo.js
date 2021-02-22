var ImageServerDemo = (function () {

    return {

        init: function (data) {

            ImageServerDemo.urls = data.urls;

            this.bindEvents();
            this.checkHealth();
        },

        bindEvents: function () {
            $("#addThumbRowBtn").click(ImageServerDemo.addThumbParam);
            $("#uploadBtn").click(ImageServerDemo.submitForm);
        },

        addThumbParam: function () {

            var templateRow = $("#thumbRowTemplate").html();
            $("#thumbsParams").append(templateRow);

        },

        getThumbParams: function () {

            var params = [];

            $.each($("#thumbsParams .row-item").get(), function () {
                var width = $(this).find(".thumb-width").val();
                var height = $(this).find(".thumb-height").val();
                var quality = $(this).find(".thumb-quality").val();

                params.push({
                    width: width ? +width : null,
                    height: height ? +height : null,
                    quality: quality ? +quality : null
                });

            });

            return params;
        },

        submitForm: function () {

            var params = ImageServerDemo.getThumbParams();

            if (!params.length) {
                alert("select params");
                return;
            }

            var paramsJson = JSON.stringify(params);

            $("#inputFile").simpleUpload(ImageServerDemo.urls.imageServerUrl + "/upload", {

                data: {
                    thumbnails: paramsJson
                },

                start: function (file) {
                    console.log("File upload started");
                },

                progress: function (progress) {
                    console.log(progress);
                },

                success: function (data) {
                    ImageServerDemo.handleUploadResult(data);
                },

                error: function (error) {
                    alert(error);
                }


            });

        },

        handleUploadResult: function (response) {

            var container = $("#resultsContainer");
            container.empty();

            var template = $("#resultItemTemplate").html();
            var compiledTemplate = Handlebars.compile(template);

            $.each(response.data.thumbnails, function (index, value) {

                container.append(compiledTemplate({
                    width: value.width,
                    height: value.height,
                    url: value.url
                }));

            });

        },

        checkHealth: function() {

            $.ajax({
                url: ImageServerDemo.urls.imageServerUrl + "/service/health"
            }).done(function() {
                $("#alertOk").removeClass("d-none");
            }).fail(function() {
                $("#alertError").removeClass("d-none");
            });

        }

    }

})();