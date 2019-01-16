<template>
  <v-footer height="auto">
    <v-card class="flex" style="background: #212121;">
      <v-card-title class="justify-center pa-0"> 
        <a href="https://github.com/yazdipour/ydm" target="blank">
            <v-btn class="green lower">
              <v-icon>smartphone</v-icon> For iOS, Android, Windows
            </v-btn>
        </a>
        <a href="https://github.com/yazdipour/ydm" target="blank">
            <v-btn class="orange">
              <v-icon>star</v-icon>Give Github Star
            </v-btn>
        </a>
      </v-card-title>
      <v-card-actions class="justify-center">
            &copy;{{ new Date().getFullYear() }}<strong style="margin-left:1em;">
            Made with <span style="font-size:1.4em;line-height: 1em;">☕</span> by <a href="https://yazdipour.github.io">Shahriar</a></strong>
      </v-card-actions>
    </v-card>
  </v-footer>
</template>
<style scoped>
a{ text-decoration:none;}
</style>