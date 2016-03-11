$scope.CopyToClipboard = function(event)
            {
                //var range = document.createRange();
                var rowHtml = event.currentTarget.parentElement.parentElement.parentElement.innerHTML;
                //debugger;

                try {
                    var body = document.body, range, sel;
                    var el = event.currentTarget.parentElement.parentElement.parentElement;
                    if (document.createRange && window.getSelection) {
                        range = document.createRange();
                        sel = window.getSelection();
                        sel.removeAllRanges();
                        try {
                            range.selectNodeContents(el);
                            sel.addRange(range);
                            //copy to clipboard
                            if (sel.anchorNode) {
                                if (window.clipboardData && clipboardData.setData) {
                                    //clipboardData.setData("Text", escape(sel.anchorNode.outerHTML));
                                    //clipboardData.setData("text/plain", escape(sel.anchorNode.outerHTML));
                                }
                            }

                        } catch (e) {
                            range.selectNode(el);
                            sel.addRange(range);
                        }
                    } else if (body.createTextRange) {
                        range = body.createTextRange();
                        range.moveToElementText(el);
                        range.select();
                        //range.execCommand("Copy");
                    }
                    //window.document.execCommand('Copy', false, text);
                    window.document.execCommand('Copy');

                    if (document.selection) {
                        var CopiedTxt = document.selection.createRange();
                        CopiedTxt.execCommand("Copy");
                    }
                }
                catch (e) {
                    alert("Sorry, your browser does not support this feature. Please right click the highlighted text and select 'copy'.")
                }
                //range.execCommand("Copy");
                //CopiedTxt.execCommand("Copy");
            }
            
            
            <span class="actioninfospan spanicons">
                        <a style="cursor: pointer;" ng-click="CopyToClipboard($event)" target="_blank">
                            <i class="icon-info-sign">
                            </i>
                        </a>
                    </span>
