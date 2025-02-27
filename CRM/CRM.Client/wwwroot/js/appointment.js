

window.appointments =
{
    config: function (element) {

        scheduler.config.details_on_dblclick = true;
        scheduler.config.details_on_create = true;

        var sText = "";
        scheduler.attachEvent("onTemplatesReady", function () {
            scheduler.templates.event_text = function (start, end, event) {

                
                    var sts = document.getElementById('appointmentstatus');
                    if (sts != undefined) {
                        if (sts.options[event.status] != undefined)
                            sText = sts.options[event.status].text;
                    }
                
                return "<b>" + event.text + "</b><br><i>" + sText + "</i>";
            },

            scheduler.templates.event_bar_text = function (start, end, event) {
                    var sts = document.getElementById('appointmentstatus');
                    if (sts != undefined) {
                        if (sts.options[event.status] != undefined)
                            sText = sts.options[event.status].text;
                }
                return "<b>" + event.text + "</b><br><i>" + sText + "</i>";
                };

            scheduler.templates.event_class = function (start, end, event) {
                var typeId = document.getElementById("typeId").value;
                var typeInt = parseInt(typeId);
                if (parseInt(event.typeId) == 1) return "marketing";
                else if (parseInt(event.typeId) == 2) return "selling";
               else return "afterSelling";
            };

        });


        //scheduler.templates.event_class = function (start, end, event) {
        //    if (event.status == 1) return "active_event";
        //    return "cancelled_event";
        //};
       
        scheduler.config.xml_date = "%Y-%m-%d %H:%i";
        scheduler.xy.bar_height = 40;
        scheduler.renderEvent = function (container, ev) {
            // your customizing code          
        }

        scheduler.init("scheduler_here", new Date(), "day");

        // load data from backend
         scheduler.load("/crm/api/events", "json");
        // connect backend to scheduler
        var dp = new scheduler.DataProcessor("/crm/api/events");
         dp.init(scheduler);
        // set data exchange mode
        dp.setTransactionMode("REST");


        var custom_form = document.getElementById("appointmentcard");

        scheduler.showLightbox = function (id) {
            const appointmentComments = document.querySelector("#appointment-comments");
            var ev = scheduler.getEvent(id);
            scheduler.startLightbox(id, custom_form);
           
            if (ev.text == 'New event') {
                ev.text = "";
                ev.selectedClient = "";
                ev.status = "0";
            }

            document.getElementById("time2")?.addEventListener("change", (event) => {
                const optionTag = event.target.querySelector(`option[value='${event.target.value}']`);
                const rawTime = optionTag.innerText;
                const splittedTime = rawTime.split(":");
                const date = new Date();
                date.setHours(splittedTime[0])
                date.setMinutes(splittedTime[1])
                date.setSeconds(0)

                if (date < new Date()) {
                    appointmentComments?.classList?.add("show")
                    appointmentComments?.classList.remove("hide")
                } else {
                    appointmentComments?.classList.add("hide")
                    appointmentComments?.classList.remove("show")
                }
                //yourSelect.options[yourSelect.selectedIndex].value
            })

            var titleElem = document.getElementById("title");

            titleElem.value = ev.text;
            var event = new Event('change');
            titleElem.dispatchEvent(event);
            document.getElementById("typeId").value = ev.typeId;
            document.getElementById("comments-after-appointent").value = ev.commentsAfterAppointent == undefined ? "" : ev.commentsAfterAppointent;
            document.getElementById("typeId").value = ev.typeId;
            document.getElementById("time1").value = ev.start_date.getHours() * 60 + ev.start_date.getMinutes();
            document.getElementById("day1").value = ev.start_date.getDate();
            document.getElementById("month1").value = ev.start_date.getMonth();
            document.getElementById("year1").value = ev.start_date.getFullYear();

            document.getElementById("time2").value = ev.end_date.getHours() * 60 + ev.end_date.getMinutes();
            document.getElementById("day2").value = ev.end_date.getDate();
            document.getElementById("month2").value = ev.end_date.getMonth();
            document.getElementById("year2").value = ev.end_date.getFullYear();
            document.getElementById('appointmentstatus').value = ev.status == undefined ? "0" : ev.status;

            if (ev.description != undefined && ev.description != "null") {
                document.getElementById("description").value = ev.description;
            }
            else {
                document.getElementById("description").value = "";
            }

            if (ev.location != undefined && ev.location != "null") {
                document.getElementById("location").value = ev.location;
            }
            else {
                document.getElementById("location").value = "";
            }

            if (new Date(ev.end_date) < new Date()) {
                appointmentComments?.classList?.add("show")
                appointmentComments?.classList.remove("hide")
            } else {
                appointmentComments?.classList.add("hide")
                appointmentComments?.classList.remove("show")
            }
         
            var elem = document.getElementById("inn");
            elem.value = ev.selectedClient == undefined ? "" : ev.selectedClient.inn;
            var event = new Event('change');
            elem.dispatchEvent(event);
        }

       

        //needs to be attached to the 'cancel' button
        //function close_form(argument) {
        //    scheduler.endLightbox(false, custom_form);

        //}

        

    },
    //needs to be attached to the 'cancel' button
    closeform: function (argument) {
        var appointmentcard = document.getElementById("appointmentcard");
        scheduler.endLightbox(false, appointmentcard);

    },

    saveform: function (model) {
        var ev = scheduler.getEvent(scheduler.getState().lightbox_id);

        if (ev == undefined) return;
        ev.text = model.text;
        ev.description = model.description;
        ev.inn = JSON.stringify(model.selectedClient.inn);
        ev.location = model.location;
        ev.status = model.status;
        ev.typeId = model.typeId;
        ev.commentsAfterAppointent = model.commentsAfterAppointent;

        ev.start_date = new Date(
            parseInt(document.getElementById("year1").value),
            parseInt(document.getElementById("month1").value),
            parseInt(document.getElementById("day1").value),
            Math.floor(parseInt(document.getElementById("time1").value) / 60) ,
                parseInt(document.getElementById("time1").value) % 60,
            0
        );

        ev.end_date = new Date(
            parseInt(document.getElementById("year2").value),
            parseInt(document.getElementById("month2").value),
            parseInt(document.getElementById("day2").value),
            Math.floor(parseInt(document.getElementById("time2").value) / 60),
                parseInt(document.getElementById("time2").value) % 60,
                0
        );

        var sts = document.getElementById('appointmentstatus');
        if (sts != undefined) {
            if (sts.options[ev.status] != undefined)
                sText = sts.options[ev.status].text;
        }
        scheduler.endLightbox(true, appointmentcard);
    },

    deleteEvent: function () {
        var event_id = scheduler.getState().lightbox_id;
        scheduler.endLightbox(false, appointmentcard);
        scheduler.deleteEvent(event_id);
    }

}
