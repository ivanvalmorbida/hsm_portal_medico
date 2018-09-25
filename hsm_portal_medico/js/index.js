var vm = new Vue({
    el: '#app',
    data: function data() {
        return {
            headers: [
                { text: 'Data e hora', align: 'left', value: 'data_hora', sortable: false },
                { text: 'Paciente', align: 'left', value: 'paciente', sortable: false },
                { text: 'Procedimento', align: 'left', value: 'procedimento', sortable: false },
                { text: 'Detalhes', value: 'item', sortable: false }
            ],
            agendas: [],

            loading: false,
            items: [],
        };
    },

    methods: {

        getAgendas() {
            /*
            this.$http.post("medico.asmx/getMedico").then((res) => {
                //this.$http.post("agenda.asmx/getAgendas", { medico: 1, anestesia: 0, tempo: 0 }).then((res) => {
                this.$http.post("agenda.asmx/getAgendas", { medico: res.data.d, anestesia: this.anestesia, tempo: this.tempo }).then((res) => {
                    //console.dir(res.data.d)
                    this.agendas = JSON.parse(res.data.d)
                })
            })*/
        },

        detalhes(item) {
            /*
            const index = this.procedimentos.indexOf(item)
            confirm('Confirma a exclusão deste item?') && this.procedimentos.splice(index, 1)
            this.totalItem()*/
        }
    },

    created() {
        this.$http.post("agenda.asmx/getAgendamentos").then((res) => {
            console.dir(res.data.d)
            this.agendas = JSON.parse(res.data.d)
        })
    }
});