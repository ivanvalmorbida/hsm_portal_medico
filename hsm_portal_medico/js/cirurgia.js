var vm = new Vue({
    el: '#app',
    data: function data() {
        return {
            guia: null, autorizacao: null, data_autoriza: null,
            data_autoriza_Formatted: null, menu_data_autoriza: false,
            valid_autoriza: null, valid_autoriza_Formatted: null,
            menu_valid_autoriza: false, tempo: null, anestesia: null, procedimento: [],

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

        data_autoriza(val) {
            this.data_autoriza_Formatted = this.formatDate(this.data_autoriza)
        },

        valid_autoriza(val) {
            this.valid_autoriza_Formatted = this.formatDate(this.valid_autoriza)
        },
    },

    methods: {
        querySelections(v) {
            if (v.length > 2) {
                this.loading = true
                this.$http.post("cirurgia.asmx/getCirurgiaNomCod", { strX: v })
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
    }
});

/*parseDate(date) {
    if (!date) return null
    const [month, day, year] = date.split('/')
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
}*/
