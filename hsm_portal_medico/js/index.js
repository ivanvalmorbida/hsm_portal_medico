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
            dialog: false,
            medico: [],
            inicio: '',
            paciente: '',
            procedimento1: ''
        };
    },

    methods: {
        detalhes(item) {
            this.dialog = true
            this.inicio = item.data_hora
            this.paciente = item.paciente
            this.procedimento1 = item.procedimento1
        }
    },

    created() {
        this.$http.post("agenda.asmx/getAgendamentos").then((res) => {
            this.agendas = JSON.parse(res.data.d)
        }),

        this.$http.post("agenda.asmx/getUsuarioDados").then((res) => {
            var tmp = JSON.parse(res.data.d)
            this.medico = tmp[0]
        })
    }
});