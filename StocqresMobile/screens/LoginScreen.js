import React, {Component} from 'react';
import { StyleSheet } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text } from 'native-base';
import {connect} from 'react-redux';
import {login} from '../ducks/login'
import {goToHome} from './navigation'

class LoginScreen extends Component{
    constructor(props){
        super(props);
        this.state = {
            login:'',
            password:''
        }
    }
    loginTo = () => {
        this.props.login(this.state.login, this.state.password).then(()=>{
            goToHome();
        })
    }
    render(){
        return(
            <Container>
                <Content contentContainerStyle={styles.container}>
                    <Form style={styles.form}>
                        <Item floatingLabel>
                            <Label>Username</Label>
                            <Input onChangeText={(value) => this.setState({login})}/>
                        </Item>
                        <Item floatingLabel>
                            <Label>Password</Label>
                            <Input onChangeText={(value) => this.setState({password})}/>
                        </Item>
                        <Button primary style={styles.submit} onPress={() => this.loginTo()}>
                            <Text>Sign In</Text>
                        </Button>
                    </Form>
                </Content>
            </Container>
        )
    }
}

const styles = StyleSheet.create({
    container:{
      flex:1,
      paddingLeft: 15,
      paddingRight: 15,
      flexDirection:'column',
      alignItems:'center',
      justifyContent:'center'
  
    },
    submit:{
      alignSelf: 'stretch',
      borderRadius: 10,
      justifyContent: 'center',
      marginTop: 40,
    },
    form: {
      alignSelf: 'stretch',
      backgroundColor: '#ffffff',
      borderColor: '#d6d7da',
      borderRadius: 10,
      borderWidth: 1,
      padding: 10,
      paddingBottom: 30,
    }
  });

  const mapDispatchToProps = {
      login
  };


const mapStateToProps = (state) => {
    return{
        username: state.login.initialState.username,
        password: state.login.initialState.password
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(LoginScreen);