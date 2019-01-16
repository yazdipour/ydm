import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'

import VueResource from 'vue-resource';
Vue.use(VueResource);

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')