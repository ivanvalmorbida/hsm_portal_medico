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
        };
    },

    methods: {
        detalhes(item) {
            this.dialog = true           
            /*
            const index = this.procedimentos.indexOf(item)
            confirm('Confirma a exclusão deste item?') && this.procedimentos.splice(index, 1)
            this.totalItem()*/
        }
    },

    created() {
        this.$http.post("agenda.asmx/getAgendamentos").then((res) => {
            this.agendas = JSON.parse(res.data.d)
        })
    }
});