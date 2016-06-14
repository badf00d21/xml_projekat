/**
 * Created by Srđan on 19.5.2016..
 */

$(function () {
    var keyValueList = window.location.search.substring(1).split('&'), params = {};
    for (var i = 0 in keyValueList) {
        if (keyValueList.hasOwnProperty(i)) {
            var keyValue = keyValueList[i].split('='); //split key and value
            params[keyValue[0]] = keyValue[1];
        }
    }

    if (params.schemaName == null || params.schemaUri == null || params.rootElement == null) {
        return;
    }

    console.log(params);

    var extractor = new Xsd2Json(params.schemaName, {
            schemaURI: params.schemaUri,
            rootElement: params.rootElement
        }),
        elementText = '<element xlmns="http://ftn.uns.ac.rs/xml"></element>'
            .split('element')
            .join(params.rootElement),
        ajaxOptions = {
            xmlUploadPath: params.submitPath
        };

    if (params.retrievalPath) {
        ajaxOptions.xmlRetrievalPath = params.retrievalPath;
    }

    $('#XMLEditor')
        .text(elementText)
        .xmlEditor({
            menuEntries: [],
            schema: extractor.getSchema(),
            ajaxOptions: ajaxOptions,
            submitErrorHandler: function (jqXHR) {
                var error = new X2JS().xml_str2json(jqXHR.responseText).error.message;
                alert(error);
            }
        });
});