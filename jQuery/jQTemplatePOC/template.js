<script id="NotificationTemplate" type="text/html">
    <tr>
        <td width='${setWidth(windowWidth, 0.1)}'>
            {{if NotificationTypeId == $item.EXCEPTION_PROJECT_DELIVERY}}
                <a href="../Projects/pgProjectTab.aspx?ProjectId=${EntityId}">${DisplayID}</a>
            {{else NotificationTypeId == $item.EXCEPTION_PROGRAM_MILESTONE}}
                <a href="../Programs/pgMainProgram.aspx?progid=${ProgramId}">${EntityId}</a>
            {{else NotificationTypeId == $item.EXCEPTION_BOW_DATA_QUALITY}}
                <a href="../Projects/pgProjectTab.aspx?ProjectId=${EntityId}">${DisplayID}</a>
            {{else}}
                ${EntityId}
            {{/if}}
        </td>
        <td width='${setWidth(windowWidth, 0.19)}' style='WORD-WRAP:BREAK-WORD'>
            {{if EntityName == null}}
                
            {{else}}
                ${EntityName}
            {{/if}}
        </td>
        
        <td align='center' width='${setWidth(windowWidth, 0.05)}' style='WORD-WRAP:BREAK-WORD'>
            {{if ( Help != null ) }}
                <img src='../Images/help_icon_small.gif' alt='${Help}'/>
            {{else}}
                
            {{/if}}
        </td>
        <td align='center' width='${setWidth(windowWidth, 0.06)}' style='WORD-WRAP:BREAK-WORD'>
            {{if (IsDummyApp != "False") }}
                {{if (NotificationTypeId == $item.EXCEPTION_BOW_DATA_QUALITY) }}
                    <input type='image' title='edit application allocation' onmouseover="this.style.cursor='hand'" style='border-width:0px;vertical-align: middle;' id='btnShowAppSelection' src='../images/edit.gif'>
                {{/if}}
            {{else}}
                {{if (IsDummyApp == "False") && (NotificationTypeId == $item.EXCEPTION_BOW_DATA_QUALITY) }}
                    {{if (NotificationCode == 'APPERR03') }}
                        <input type='image' title='Ensure your FPC is linked to an approved SOW' onmouseover="this.style.cursor='hand'" style='border-width:0px;vertical-align: middle;' id='btnSOWAssignment' src='../images/Dollar.png'>
                    {{/if}}
                    
                {{/if}}
            {{/if}}
            {{if (NotificationTypeId == $item.EXCEPTION_PROJECT_DELIVERY) }}
                {{if (ArtifactTypeId == "1") || (ArtifactTypeId == "5") }}
                    {{if (!((TaskId == 321) || (TaskId == 351) || (TaskId == 331) || (TaskId == 411) || (TaskId == 626) || (TaskId == 613) || (TaskId == 688))) }}
                        <input type='image' title='Upload Documents' onmouseover="this.style.cursor='hand'" style='border-width:0px;vertical-align: middle;' id='btnUploadExceptionDocuments' src='../images/UploadDoc.png'>
                    {{else}}
                        
                    {{/if}}
                {{/if}}
            {{/if}}
        </td>
    </tr>
    </script>
    
    // global variables
    var GlobalVars = {
            EXCEPTION_PROJECT_DELIVERY: "ProjDeliverables",
            ....
            .....
            
            windowWidth: 0
        };
        
        $(document).ready(function () {
           
            $.template("cachedNotificationTemplate", $('#NotificationTemplate'));
    ...
    ...
