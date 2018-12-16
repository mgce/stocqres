import React, {Component} from 'react';
import {connect} from 'react-redux';
import { goToAuth, goToHome } from './navigation'
import { View, Text } from 'native-base';

class Initialize extends React.PureComponent{
    async componentDidMount(){
        try{
            if(this.props.accessToken !== null && this.props.accessToken !== undefined && this.props.accessToken !== '')
                goToHome();
            else
                goToAuth(); 
        }
        catch(err){
            console.log('error: ' + err);
            goToAuth();
        }
    }
    render(){
        return(
            <View>
                <Text>Loading</Text>
            </View>
        )
    }
}

const mapStateToProps = (state) => {
    return{
        accessToken: state.authentication.accessToken
    }
}

export default connect(mapStateToProps)(Initialize);