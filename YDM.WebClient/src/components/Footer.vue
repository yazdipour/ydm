<template>
  <v-footer height="auto">
    <v-card class="flex" style="background: #212121;">
      <v-card-title class="justify-center pa-0"> 
        <a href="https://github.com/yazdipour/ydm" target="blank">
            <v-btn class="green">
              <v-icon>android</v-icon> Get for iOS, Android, Windows
            </v-btn>
        </a>
        <a href="https://github.com/yazdipour/ydm" target="blank">
            <v-btn class="orange">
              <v-icon>star</v-icon> Star YDM on Github
            </v-btn>
        </a>
      </v-card-title>
      <v-card-actions class="justify-center">
            &copy; {{ new Date().getFullYear() }}<strong style="margin-left:1em;">YDM</strong>
      </v-card-actions>
    </v-card>
  </v-footer>
</template>
<style scoped>
a{ text-decoration:none;}
</style>