var vm = new Vue({
    el: '#app',
    data: function data() {
        return {
            guia: null, autorizacao: null, data_autorizacao: null,
            data_autorizacao_Formatted: null, menu_data_autorizacao: false,
            validade_autorizacao: null, validade_autorizacao_Formatted: null,
            menu_validade_autorizacao: false, tempo: null, anestesia: null, procedimento: [],

            headers: [
                { text: 'Procedimento', align: 'left', value: 'procedimento' },
                { text: 'Descrição', align: 'left', value: 'descricao' },
                { text: 'Excluir', value: 'procedimento', sortable: false }
            ],
            procedimentos: [],

            loading: false,
            items: [],
            search: null,
        };
    },

    watch: {
        search(val) {
            val && this.querySelections(val)
        },

        data_autorizacao(val) {
            this.data_autorizacao_Formatted = this.formatDate(this.data_autorizacao)
        },

        validade_autorizacao(val) {
            this.validade_autorizacao_Formatted = this.formatDate(this.validade_autorizacao)
        },
    },

    methods: {
        querySelections(v) {
            if (v.length > 2) {
                this.loading = true
                this.$http.post("cirurgia.asmx/getCirurgiaNom", { strNom: v })
                    .then((res) => {
                        var r = JSON.parse(res.data.d)
                        this.loading = false
                        this.items = r
                    })
            }
        },

        delItem(item) {
            const index = this.procedimentos.indexOf(item)
            confirm('Confirma a exclusão deste item?') && this.procedimentos.splice(index, 1)
        },

        addItem() {
            this.procedimentos.push({
                'procedimento': this.procedimento.value, 'descricao': this.procedimento.nome
            })
            this.procedimento = null
        },

        formatDate(date) {
            if (!date) return null

            const [year, month, day] = date.split('-')
            return `${day}/${month}/${year}`
        }

        /*parseDate(date) {
            if (!date) return null
            const [month, day, year] = date.split('/')
            return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
        }*/
    }
});