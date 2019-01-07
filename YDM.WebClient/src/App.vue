<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar
        color="red darken-2"
        extended
        flat>
        <v-card-text>
          <h1 class="text-md-center display-1 font-weight-light">YDM</h1>
        </v-card-text>
      </v-toolbar>
  
      <v-layout row pb-2 style="background: #303030;">
        <v-flex xs8 offset-xs2>
          <v-card class="card--flex-toolbar" style="margin-top: -64px;background: #212121;">
            <v-toolbar card prominent>
              <v-text-field color="white" label="Youtube Video URL / Search / PlayList" style="margin-top: 12px"></v-text-field>
              <v-spacer></v-spacer>
              <v-btn round color="red" dark><v-icon>get_apps</v-icon>GET</v-btn>
              <v-btn round color="#444"><v-icon>search</v-icon>Search</v-btn>
            </v-toolbar>
            <v-divider></v-divider>
            <v-card-text style="height: 200px;">
            
              <v-btn icon><v-icon>invert_colors</v-icon></v-btn>
              <v-btn icon><v-icon>flip_to_front</v-icon></v-btn>
              <v-btn icon><v-icon>important_devices</v-icon></v-btn>
              <v-btn icon><v-icon>launch</v-icon></v-btn>
              <v-btn icon><v-icon>play_circle_filled</v-icon></v-btn>
              <v-btn icon><v-icon>link</v-icon></v-btn>
            
            </v-card-text>

            <!-- link to App and Github Pages-->
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  </v-app>
</template>

<script>
  export default {
    data: () => ({
      drawer: null
    }),

    props: {
      source: String
    }
  }
</script>